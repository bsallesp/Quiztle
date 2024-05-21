import os
import time
import psutil
import json
import uuid
from flask import Flask, request, jsonify
import logging
from werkzeug.exceptions import BadRequest
from pdf2image import convert_from_path
import pytesseract
from PIL import Image
import re

app = Flask(__name__)

# Configure logging
logging.basicConfig(level=logging.DEBUG)

def clean_text(text):
    # Substitui múltiplos '\n' e espaços em branco por um único espaço
    cleaned_text = re.sub(r'\s+', ' ', text).strip()
    return cleaned_text

def get_memory_usage():
    process = psutil.Process(os.getpid())
    return process.memory_info().rss  # Returns memory usage in bytes

def extract_text_from_pdf(pdf_path, partial_output_rate):
    logging.debug("Converting PDF to images...")
    images = convert_from_path(pdf_path)
    extracted_text = ""

    for index, image in enumerate(images):
        if (index + 1) % partial_output_rate == 0:
            logging.debug(f"Processing page {index + 1}...")
            text = pytesseract.image_to_string(image)
            extracted_text += text + "\n\n"
            logging.debug(f"Page {index + 1} processed.")

    # Limpa o texto antes de retornar
    return clean_text(extracted_text)

@app.route('/extract-text', methods=['POST'])
def extract_text():
    try:
        start_time = time.time()
        start_memory = get_memory_usage()

        file_path = request.form.get('file_path')
        user_uuid = request.form.get('uuid', str(uuid.uuid4()))
        partial_output_rate = int(request.form.get('partial_output_rate', 1))

        if not file_path or not os.path.exists(file_path):
            raise BadRequest("No valid file path provided or file does not exist.")

        # Extração do nome original do arquivo a partir do caminho do arquivo
        original_filename = os.path.basename(file_path)

        extracted_text = extract_text_from_pdf(file_path, partial_output_rate)

        end_time = time.time()
        end_memory = get_memory_usage()
        time_taken = end_time - start_time
        memory_used = (end_memory - start_memory) / (1024 * 1024)  # Convert to MB

        output_dir = os.getenv('OUTPUT_FILES', '/bucket/output')
        output_filename = f"{user_uuid}.json"
        output_filepath = os.path.join(output_dir, output_filename)

        result = {
            "original_filename": original_filename,  # Inclui o nome do arquivo original no resultado
            "extracted_text": extracted_text,
            "time_taken": f"{time_taken:.2f} seconds",
            "memory_used": f"{memory_used:.2f} MB"
        }

        with open(output_filepath, "w", encoding='utf-8') as output_file:
            json.dump(result, output_file, ensure_ascii=False, indent=4)

        return jsonify({"output_file": output_filepath})

    except BadRequest as e:
        logging.error(f"BadRequest: {str(e)}")
        return jsonify({"error": str(e)}), 400
    except Exception as e:
        logging.error(f"Unhandled exception: {str(e)}")
        return jsonify({"error": "An unexpected error occurred"}), 500

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
