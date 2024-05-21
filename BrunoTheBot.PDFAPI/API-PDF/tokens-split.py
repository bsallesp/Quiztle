from transformers import GPT2Tokenizer

def split_text_into_token_chunks(text, max_tokens_per_chunk):
    # Inicializa o tokenizer do GPT-2, que é similar ao do GPT-3 e GPT-4
    tokenizer = GPT2Tokenizer.from_pretrained('gpt2')

    # Tokeniza o texto
    tokens = tokenizer.encode(text)

    # Dividir os tokens em chunks
    chunks = []
    for i in range(0, len(tokens), max_tokens_per_chunk):
        # Converte os tokens de volta para texto
        chunk_text = tokenizer.decode(tokens[i:i + max_tokens_per_chunk])
        chunks.append(chunk_text)

    return chunks

# Exemplo de uso
text = "Este é um exemplo de texto que será dividido em tokens segundo o modelo de tokenização do GPT. " \
       "Cada token é contado de acordo com as regras de tokenização específicas do modelo, " \
       "incluindo espaços, pontuações e estruturas de palavras."
chunks = split_text_into_token_chunks(text, 10)
print(chunks)
