FROM prom/prometheus

COPY ./monitor/prometheus/prometheus.yml /etc/prometheus/prometheus.yml

CMD ["--config.file=/etc/prometheus/prometheus.yml", "--storage.tsdb.path=/prometheus"]

EXPOSE 9090