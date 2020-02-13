from typing import Optional, List, Dict
import json
from cryptography.fernet import Fernet
from config import token_key
import base64

# str to byte[]
f = Fernet(token_key.encode())


def generate_token(payload: Dict) -> str:
    secret = json.dumps(payload)
    # byte[] to str
    return f.encrypt(secret.encode()).decode()


def decode_token(token: str) -> Dict:
    # decrypt requires str to bytes[]
    # and bytes[] to str the result
    decrypted: str = f.decrypt(token.encode()).decode()
    return json.loads(decrypted)
