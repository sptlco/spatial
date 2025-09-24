#!/bin/bash

exec code-server ${PROJECT_DIR} \
    --user-data-dir "${DATA_DIR}" \
    --config "${CONFIG_DIR}/config.yaml"
