#!/bin/bash

exec code-server --bind-addr 0.0.0.0:8080 \
    --auth password \
    --user-data-dir "${DATA_DIR}" \
    --config "${CONFIG_DIR}/config.yaml" \
    "${PROJECT_DIR}"
