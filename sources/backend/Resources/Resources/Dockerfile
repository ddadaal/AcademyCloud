##############
# Builder    #
##############
FROM python:3 AS builder

# This is where pip will install to
ENV PYROOT /pyroot
# A convenience to have console_scripts in PATH
ENV PATH $PYROOT/bin:$PATH
ENV PYTHONUSERBASE $PYROOT

# THE MAIN COURSE #

WORKDIR /build

# Install pipenv
RUN pip install pipenv -i https://pypi.tuna.tsinghua.edu.cn/simple some-package

# Install dependencies
COPY Pipfile Pipfile.lock ./
RUN PIP_USER=1 PIP_IGNORE_INSTALLED=1 pipenv install --system --deploy

##############
# Production #
##############

FROM python:3 AS prod

ENV PYROOT /pyroot
ENV PATH $PYROOT/bin:$PATH
ENV PYTHONPATH $PYROOT/lib/python:$PATH
# This is crucial for pkg_resources to work
ENV PYTHONUSERBASE $PYROOT

# Finally, copy artifacts
COPY --from=builder $PYROOT/lib/ $PYROOT/lib/

# Copy source code
COPY . ./

# Wait for database to be available
# Then run, -u for unbuffered output
CMD ["./wait-for-it.sh", "db:3306", "-t", "60000", "--", "python", "-u", "app.py"]



