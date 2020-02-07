from typing import Optional

from cryptography.fernet import Fernet
from connection.connection import ScopedAuth
from config import token_key

# Put this somewhere safe!
# str to byte[]
f = Fernet(token_key.encode())

encoding = "utf-8"


def generate_token(auth: ScopedAuth) -> str:
    secret = "{}+{}+{}+{}".format(auth.username,
                                  auth.password,
                                  auth.domain_name,
                                  auth.project_name if auth.project_name else ""
                                  ).encode(encoding)
    # byte[] to str
    return f.encrypt(secret).decode()


def decode_token(token: str) -> ScopedAuth:
    # decrypt requires str to bytes[]
    # and bytes[] to str the result
    decrypted: str = f.decrypt(token.encode()).decode()
    splited = decrypted.split("+")
    return ScopedAuth(
        username=splited[0],
        password=splited[1],
        domain_name=splited[2],
        project_name=splited[3] if splited[3] == "" else None
    )
