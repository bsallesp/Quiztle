import os
import logging
import time
import uuid
import json
import re
from flask import Flask, request, jsonify, Response, abort
from werkzeug.exceptions import BadRequest
from pdf2image import convert_from_path
import pytesseract
from PIL import Image
import fitz  # PyMuPDF
import psutil

# Configuração inicial do Flask e do logging
app = Flask(__name__)
app.config['PDF_DIRECTORY'] = os.getenv('PDF_DIRECTORY', '/bucket')
logging.basicConfig(level=logging.DEBUG)

def clean_text(text):
    """Limpa o texto extraído, removendo espaços extras e quebras de linha."""
    text = re.sub(r'\s+', ' ', text).strip()
    logging.debug(f"Cleaned text: {text[:100]}")  # Log the first 100 characters of cleaned text
    return text

def get_memory_usage():
    """Retorna o uso atual de memória do processo."""
    process = psutil.Process(os.getpid())
    memory_usage = process.memory_info().rss
    logging.debug(f"Current memory usage: {memory_usage} bytes")
    return memory_usage

def extract_text_from_pdf(pdf_path, partial_output_rate):
    """Extrai texto de um PDF convertendo-o em imagens e usando OCR."""
    logging.debug(f"Starting PDF to image conversion for {pdf_path}")
    try:
        images = convert_from_path(pdf_path, dpi=300)
        logging.debug(f"Converted PDF to {len(images)} images")
    except Exception as e:
        logging.error(f"Failed to convert PDF to images: {e}")
        raise

    extracted_texts = []
    for index, image in enumerate(images):
        if (index + 1) % partial_output_rate == 0:
            logging.debug(f"Processing page {index + 1}")
            try:
                text = pytesseract.image_to_string(image)
                cleaned_text = clean_text(text)
                extracted_texts.append(cleaned_text)
                logging.debug(f"Page {index + 1} processed")
            except Exception as e:
                logging.error(f"Failed to process page {index + 1}: {e}")
                continue
    return extracted_texts

@app.route('/extract-text-mupdf/<filename>/<int:partial_output_rate>', methods=['GET'])
def extract_text_mupdf(filename, partial_output_rate):
    """API para extrair texto de PDF usando MuPDF e pytesseract para OCR em caso de texto vazio."""
    file_path = os.path.join(app.config['PDF_DIRECTORY'], filename)
    if not os.path.exists(file_path):
        error_message = f"Invalid file path or file does not exist: {file_path}"
        logging.error(error_message)
        abort(400, description=error_message)

    def generate():
        logging.debug(f"Opening PDF file: {file_path}")
        try:
            doc = fitz.open(file_path)
            logging.debug(f"Opened PDF with {doc.page_count} pages")
            for index, page in enumerate(doc):
                if (index + 1) % partial_output_rate == 0:
                    text = page.get_text()
                    if not text.strip():
                        pix = page.get_pixmap()
                        img = Image.frombytes("RGB", [pix.width, pix.height], pix.samples)
                        text = pytesseract.image_to_string(img)
                    cleaned_text = clean_text(text)
                    yield f"data:Page {index + 1}: {cleaned_text}\n\n"
                    logging.debug(f"Streamed text for page {index + 1}")
        except Exception as e:
            error_detail = f"Failed to open or read PDF file at {file_path}: {str(e)}"
            logging.error(error_detail)
            abort(400, description=error_detail)
        finally:
            doc.close()

    return Response(generate(), mimetype='text/event-stream')

@app.route('/list-pdfs', methods=['GET'])
def list_pdfs():
    """Lista todos os arquivos PDF no diretório configurado."""
    pdf_directory = app.config['PDF_DIRECTORY']
    if not os.path.exists(pdf_directory):
        logging.error("PDF directory does not exist.")
        abort(404, description="PDF directory does not exist.")

    pdf_files = [f for f in os.listdir(pdf_directory) if f.lower().endswith('.pdf')]
    pdf_files_with_paths = [os.path.join(pdf_directory, f) for f in pdf_files]
    
    logging.debug(f"Found PDF files: {pdf_files}")
    logging.debug(f"PDF files with paths: {pdf_files_with_paths}")

    return jsonify({
        "pdf_files": pdf_files,
        "pdf_files_with_paths": pdf_files_with_paths
    })   

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
