# AcademyCloud Resources API

API for AcademyCloud resources.

## Features

- Python 3.8
- [pipenv](https://pipenv.kennethreitz.org/en/latest/) for dependency management
- Complete [type hints](https://docs.python.org/3/library/typing.html)
- [flask](https://flask.palletsprojects.com/en/1.1.x/) and [flask-RESTful](https://flask-restful.readthedocs.io/en/latest/index.html) 
- [SQLAlchemy](https://www.sqlalchemy.org/) with [flask-sqlalchemy](https://flask-sqlalchemy.palletsprojects.com/en/2.x/)

## Pipenv and vim and coc-python

- `pipenv install --dev` to install both dev and prod packages
- `python shell` to enter venv
- `nvim-qt .` to start vim

## Make sure the connectivity from project machine to openstack

The authentication to openstack identity returns the catalog of server endpoints, which are the addresses to each server.

If the machine your openstack is running on is different to the machine this project is running, you must ensure that it is accessible from the project machine to the openstack machine.
