# AcademyCloud backend

The backend for AcademyCloud.

# Features

- Heterogeneous microservices with C# and Python which communites with normal HTTP and JSON
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