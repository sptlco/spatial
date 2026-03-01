#!/usr/bin/env bash
set -e

PROJECT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )/.." && pwd )"

echo "Installing..."

sudo tee /usr/local/bin/spatial > /dev/null <<EOF
#!/usr/bin/env bash
set -e

PROJECT_ROOT="$PROJECT_ROOT"
VERSION="1"

# Detect if terminal supports color
if [ -t 1 ]; then
  RED="\033[0;31m"
  GREEN="\033[0;32m"
  BLUE="\033[0;34m"
  CYAN="\033[0;36m"
  YELLOW="\033[1;93m"
  BOLD="\033[1m"
  RESET="\033[0m"
else
  RED=""
  GREEN=""
  BLUE=""
  CYAN=""
  BOLD=""
  RESET=""
fi

print_logo() {
cat <<'ART'

┌─────────────────────────────────────────┐
│                                         │
│      _____             __  _       __   │
│     / ___/____  ____ _/ /_(_)___ _/ /   │
│     \__ \/ __ \/ __ \`/ __/ / __ \`/ /    │
│    ___/ / /_/ / /_/ / /_/ / /_/ / /     │
│   /____/ .___/\__,_/\__/_/\__,_/_/      │
│       /_/                               │
│                                         │
│                                         │
└─────────────────────────────────────────┘

ART
}

success() {
  echo -e " \${GREEN}✔ \$1\${RESET}"
}

info() {
  echo -e " → \$1"
}

error() {
  echo -e " \${RED}✖ \$1\${RESET}"
}

show_help() {
  print_logo
  echo -e "\${YELLOW}Usage:\${RESET}"
  info "spatial <command>"
  echo
  echo -e "\${YELLOW}Commands:\${RESET}"
  info "deploy      Deploy the stack"
  info "shutdown    Shutdown the stack"
  info "redeploy    Redeploy the stack"
  info "develop     Develop the stack"
  info "status      Get the current status"
  info "logs        Follow logs"
  info "help        Get help"
  echo
}

deploy_stack() {
  print_logo
  info "Deploying..."
  echo

  docker compose -p spatial -f "$PROJECT_ROOT/docker-compose.yml" up -d --build

  echo
  success "Done."
  echo
}

shutdown_stack() {
  print_logo
  info "Shutting down..."
  echo

  docker compose -p spatial -f "$PROJECT_ROOT/docker-compose.yml" down

  echo
  success "Done."
  echo
}

redeploy_stack() {
  print_logo
  info "Redeploying..."
  echo

  docker compose -p spatial -f "$PROJECT_ROOT/docker-compose.yml" up -d --build --remove-orphans

  echo
  info "Reloading NGINX..."
  echo
  
  docker compose -p spatial -f "$PROJECT_ROOT/docker-compose.yml" exec nginx nginx -s reload

  echo
  success "Done."
  echo
}

dev_mode() {
  print_logo

  cleanup() {
    echo
    info "Shutting down..."
    echo
    kill \$PID 2>/dev/null || true
    wait
    echo
    success "Done."
    echo
    exit 0
  }

  trap cleanup SIGINT SIGTERM

  (cd "\$PROJECT_ROOT/cloud/Server" && ASPNETCORE_ENVIRONMENT=Development dotnet watch) &
  PID=\$!

  success "Development environment running..."
  info "Press Ctrl+C to shutdown."
  echo

  wait
}

get_status() {
  print_logo

  docker compose -p spatial -f "$PROJECT_ROOT/docker-compose.yml" ps

  echo
}

follow_logs() {
  print_logo

  docker compose -p spatial -f "$PROJECT_ROOT/docker-compose.yml" logs -f
}

case "\$1" in
  deploy)
    deploy_stack
    ;;
  shutdown)
    shutdown_stack
    ;;
  redeploy)
    redeploy_stack
    ;;
  dev|develop)
    dev_mode
    ;;
  status)
    get_status
    ;;
  logs)
    follow_logs
    ;;
  help|"")
    show_help
    ;;
  *)
    echo
    error "Unknown command: \$1"
    show_help
    exit 1
    ;;
esac
EOF

sudo chmod +x /usr/local/bin/spatial

echo "Done."

spatial