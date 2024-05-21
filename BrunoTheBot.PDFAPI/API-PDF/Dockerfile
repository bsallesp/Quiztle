# Usar uma imagem base do Python
FROM python:3.8-slim

# Definir o diretório de trabalho no contêiner
WORKDIR /app

# Copiar os arquivos de dependências e instalar as bibliotecas
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copiar o restante dos arquivos da aplicação para o contêiner
COPY . .

# Expor a porta que o Flask vai usar
EXPOSE 5000

# Definir variáveis de ambiente
ENV FLASK_APP=app.py
ENV FLASK_RUN_HOST=0.0.0.0
ENV FLASK_ENV=development

# Comando para executar a aplicação
CMD ["flask", "run"]
