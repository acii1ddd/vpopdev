FROM postgres:17.0

RUN apt update \
    && apt install -y --no-install-recommends \
       patroni \
       python3-psycopg2 \
    && apt clean \
    && rm -rf /var/lib/apt/lists/* /var/cache/* \
    && mkdir -p /var/lib/postgresql \
    && chown postgres:postgres /var/lib/postgresql/\
    && chmod 700 /var/lib/postgresql/

# устанавливаем в кач-ве активного юзера внутри контейнера
#USER postgres

#ENTRYPOINT ["/usr/bin/patroni"]
#CMD [ "/etc/patroni/config.yml" ]

#CMD ["tail", "-f", "/dev/null"]
