# Usar uma imagem base do Python
FROM python:3.8-slim

# Instalar depend�ncias do sistema, incluindo Poppler e Tesseract
RUN apt-get update && apt-get install -y \
    poppler-utils \
    tesseract-ocr \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# Definir o diret�rio de trabalho no cont�iner
WORKDIR /app

# Copiar os arquivos de depend�ncias e instalar as bibliotecas
COPY requirements.txt requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Copiar o restante dos arquivos da aplica��o para o cont�iner
COPY . .

# Expor a porta que o Flask vai usar
EXPOSE 5000

# Definir vari�veis de ambiente
ENV FLASK_APP=app.py
ENV FLASK_RUN_HOST=0.0.0.0
ENV FLASK_ENV=production

# Comando para executar a aplica��o
CMD ["flask", "run"]


#docker build -f C:\Users\bsall\source\repos\Quiztle\Quiztle.Flask.PDFScanner\Dockerfile -t quiztle-flask-pdfscanner .
