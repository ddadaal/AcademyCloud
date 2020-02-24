FROM python:3

COPY . /app
WORKDIR /app

RUN pip install pipenv -i https://pypi.tuna.tsinghua.edu.cn/simple

RUN pipenv install --system --deploy

CMD ["python", "app.py"]