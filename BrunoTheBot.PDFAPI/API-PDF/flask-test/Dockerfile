# Use uma imagem base do Python
FROM python:3.8-slim-buster

# Defina o diretório de trabalho no contêiner
WORKDIR /app

# Copie os arquivos do aplicativo para o contêiner
COPY . .

# Instale as dependências
RUN pip install flask

# Exponha a porta que o aplicativo usa
EXPOSE 80

# Execute o aplicativo
CMD ["python", "app.py"]
