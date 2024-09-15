from datetime import datetime
from flask import Flask, jsonify
import threading
import time
import os
from dotenv import load_dotenv

app = Flask(__name__)

# Carregar variáveis de ambiente do arquivo .env
load_dotenv()

# Caminho para o arquivo .env
ENV_FILE = '.env'

# Função para ler o intervalo do arquivo .env
def read_interval_from_env():
    load_dotenv()
    return int(os.getenv('INTERVAL', 2))

# Função para escrever o intervalo no arquivo .env
def write_interval_to_env(seconds):
    with open(ENV_FILE, 'r') as file:
        lines = file.readlines()
    with open(ENV_FILE, 'w') as file:
        for line in lines:
            if line.startswith('INTERVAL'):
                file.write(f'INTERVAL={seconds}\n')
            else:
                file.write(line)
                
# Variáveis globais
interval = read_interval_from_env()
interval_lock = threading.Lock()  # Para sincronização de acesso ao intervalo

def GetLLMResult():
    """Função que será executada a cada X segundos e altera o intervalo para X + 1."""
    global interval
    current_time = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    print(f"{current_time} - Interval: {interval} seconds - Executing GetLLMResult...")
    
    # Atualiza o intervalo para X + 1
    new_interval = interval + 1
    with interval_lock:
        write_interval_to_env(new_interval)
        interval = new_interval

def repeat_function():
    """Função que repete a execução de GetLLMResult e atualiza o intervalo."""
    global interval
    while True:
        interval = read_interval_from_env()  # Atualiza o intervalo a partir do arquivo .env
        time.sleep(interval)  # Aguarda o intervalo
        GetLLMResult()

@app.route('/api/seconds/<int:seconds>', methods=['GET'])
def set_seconds(seconds):
    """Rota para definir o valor inicial de X."""
    global interval
    with interval_lock:
        write_interval_to_env(seconds)
        interval = seconds
    return jsonify({"message": f"Initial interval set to {seconds} seconds."})

if __name__ == '__main__':
    # Iniciar a thread para repetir a função
    thread = threading.Thread(target=repeat_function)
    thread.daemon = True
    thread.start()
    # Rodar o app Flask na porta 5000
    app.run(port=5000)
