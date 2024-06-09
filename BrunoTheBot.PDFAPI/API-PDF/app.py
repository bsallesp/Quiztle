import os
import time
import psutil
import json
import uuid
from flask import Flask, request, jsonify, Response
import logging
from werkzeug.exceptions import BadRequest
from pdf2image import convert_from_path
import pytesseract
from PIL import Image
import re
import fitz

app = Flask(__name__)
logging.basicConfig(level=logging.DEBUG)

def clean_text(text):
    text = re.sub(r'\s+', ' ', text).strip()
    logging.debug(f"Cleaned text: {text[:100]}")  # Log the first 100 characters of cleaned text
    return text

def get_memory_usage():
    process = psutil.Process(os.getpid())
    memory_usage = process.memory_info().rss
    logging.debug(f"Current memory usage: {memory_usage} bytes")
    return memory_usage

def extract_text_from_pdf(pdf_path, partial_output_rate):
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

@app.route('/extract-text-mupdf', methods=['POST'])
def extract_text_mupdf():
    file_path = request.form.get('file_path')
    partial_output_rate = int(request.form.get('partial_output_rate', 1))

    if not file_path or not os.path.exists(file_path):
        error_message = f"Invalid file path or file does not exist: {file_path}"
        logging.error(error_message)
        raise BadRequest(error_message)

    def generate():
        logging.debug(f"Opening PDF file: {file_path}")
        try:
            doc = fitz.open(file_path)
            logging.debug(f"Opened PDF with {doc.page_count} pages")
            for index, page in enumerate(doc):
                if (index + 1) % partial_output_rate == 0:
                    text = page.get_text()
                    if not text.strip():  # Check if text is empty
                        # Convert page to image and use OCR
                        pix = page.get_pixmap()
                        img = Image.frombytes("RGB", [pix.width, pix.height], pix.samples)
                        text = pytesseract.image_to_string(img)
                    cleaned_text = clean_text(text)
                    yield f"data:Page {index + 1}: {cleaned_text}\n\n"
                    logging.debug(f"Streamed text for page {index + 1}")
        except Exception as e:
            error_detail = f"Failed to open or read PDF file at {file_path}: {str(e)}"
            logging.error(error_detail)
            raise BadRequest(error_detail)
        finally:
            doc.close()

    return Response(generate(), mimetype='text/event-stream')

@app.route('/list-pdfs', methods=['GET'])
def list_pdfs():
    pdf_directory = '/app/bucket/pdf-files'

    if not os.path.exists(pdf_directory):
        logging.error("PDF directory does not exist.")
        raise BadRequest("PDF directory does not exist.")

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
