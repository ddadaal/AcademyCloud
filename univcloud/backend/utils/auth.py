from typing import Optional, List

from cryptography.fernet import Fernet
from client.client import ScopedAuth
from config import token_key

# Put this somewhere safe!
# str to byte[]
f = Fernet(token_key.encode())


def generate_token(auth: ScopedAuth) -> str:
    secret = "{}+{}+{}+{}".format(auth.username,
                                  auth.password,
                                  auth.domain_name,
                                  auth.project_name if auth.project_name else ""
                                  )
    # byte[] to str
    return f.encrypt(secret.encode()).decode()


def decode_token(token: str) -> ScopedAuth:
    # decrypt requires str to bytes[]
    # and bytes[] to str the result
    decrypted: str = f.decrypt(token.encode()).decode()
    parts: List[str] = decrypted.split("+")
    return ScopedAuth(
        username=parts[0],
        password=parts[1],
        domain_name=parts[2],
        project_name=parts[3] if parts[3] != "" else None
    )
