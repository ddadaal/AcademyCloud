# AcademyCloud backend

The backend for AcademyCloud.

# Features

- Heterogeneous microservices with C# and Python which communites via gRPC
- `docker-compose`
- `consul` for service registration and discovery
- `registrator` to automatically register containers when up and de-register when down
- C# services `API`, `Expenses` and `Identity` features:
    - **Domain Driven Design** with strictly no dependencies from domain models to anything else
    - **C# 8** with **nullable reference type** enabled for null safety
- Python service `Resources` to communicate with OpenStack and features:
    - Python 3.8
    - [pipenv](https://pipenv.kennethreitz.org/en/latest/) for dependency management
    - Complete [type hints](https://docs.python.org/3/library/typing.html)
    - [flask](https://flask.palletsprojects.com/en/1.1.x/) and [flask-RESTful](https://flask-restful.readthedocs.io/en/latest/index.html) 
    - [SQLAlchemy](https://www.sqlalchemy.org/) with [flask-sqlalchemy](https://flask-sqlalchemy.palletsprojects.com/en/2.x/)


# Run

Visual Studio 2019 supports both C# and Python projecs well.

# HTTPS (Help wanted)

API is configured to use HTTPS to serve HTTP RESTful APIs for frontend.

The gRPC communication between microservices in **insecure** and is through http (instead of https) port, because communication through https does not work in my environment:

I can confirm that HTTPS is configured correctly for each microservices, for each listens at `https://[::]:443` correctly and dev certs are trusted, but when the channel is on HTTPS address, calling gRPC interfaces prints `TLS connection could not be established` and failed.

Any help is appreciated.
