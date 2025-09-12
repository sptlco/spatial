echo ""
echo -e "\033[0m\033[1;33m"
echo "   ███████╗██████╗  █████╗ ████████╗██╗ █████╗ ██╗      "
echo "   ██╔════╝██╔══██╗██╔══██╗╚══██╔══╝██║██╔══██╗██║      "
echo "   ███████╗██████╔╝███████║   ██║   ██║███████║██║      "
echo "   ╚════██║██╔═══╝ ██╔══██║   ██║   ██║██╔══██║██║      "
echo "   ███████║██║     ██║  ██║   ██║   ██║██║  ██║███████╗"
echo "   ╚══════╝╚═╝     ╚═╝  ╚═╝   ╚═╝   ╚═╝╚═╝  ╚═╝╚══════╝"
echo -e "\033[0m"
echo ""

SCRIPT_PATH=$(dirname "$(realpath -s "$0")")

cd "$SCRIPT_PATH"
cd ..

set -e

# Detect operating system
OS="$(uname -s)"
case "$OS" in
  Linux*)     MACHINE=linux;;
  Darwin*)    MACHINE=mac;;
  CYGWIN*)    MACHINE=win;;
  MINGW*)     MACHINE=win;;
  *)          MACHINE="unknown"
esac

install_vscode() {
    printf "\t1. VS Code "
    exec >/dev/null

    if [ "$MACHINE" = "win" ]; then
        if ! winget list --id Microsoft.VisualStudioCode > /dev/null 2>&1; then
            winget install -e --id Microsoft.VisualStudioCode
        fi
    elif [ "$MACHINE" = "linux" ]; then
        if ! snap list code > /dev/null 2>&1; then
            sudo snap install --classic code
        fi
    elif [ "$MACHINE" = "mac" ]; then
        if ! brew list --cask visual-studio-code > /dev/null 2>&1; then
            brew install --cask visual-studio-code
        fi
    fi

    code --install-extension ms-dotnettools.vscode-dotnet-runtime
    code --install-extension ms-vscode.cpptools
    code --install-extension ms-vscode.cpptools-extension-pack
    code --install-extension ms-dotnettools.csharp
    code --install-extension ms-dotnettools.csdevkit
    code --install-extension EditorConfig.EditorConfig
    code --install-extension unifiedjs.vscode-mdx
    code --install-extension bradlc.vscode-tailwindcss
    code --install-extension redhat.vscode-yaml

    mkdir -p .vscode
cat << EOF > .vscode/settings.json
{
    "workbench.startupEditor": "readme"
}
EOF

    exec >/dev/tty
    printf "✔\n"
}

install_node() {
    printf "\t2. Node "
    exec >/dev/null

    if ! command -v node > /dev/null 2>&1; then
        if [ "$MACHINE" = "win" ]; then
            winget install -e --id OpenJS.NodeJS.LTS
        elif [ "$MACHINE" = "linux" ]; then
            curl -fsSL https://deb.nodesource.com/setup_lts.x | sudo -E bash -
            sudo apt-get install -y nodejs
        elif [ "$MACHINE" = "mac" ]; then
            brew install node
        fi
    fi

    exec >/dev/tty
    printf "✔\n"
}

install_dotnet() {
    printf "\t3. .NET "
    exec >/dev/null

    if ! command -v dotnet > /dev/null 2>&1 || ! dotnet --list-sdks | grep -q '9.0'; then
        if [ "$MACHINE" = "win" ]; then
            winget install -e --id Microsoft.DotNet.SDK.9
        elif [ "$MACHINE" = "linux" ]; then
            if [ ! -f /etc/apt/sources.list.d/microsoft-prod.list ]; then
                wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
                sudo dpkg -i packages-microsoft-prod.deb
                rm packages-microsoft-prod.deb
            fi

            sudo apt-get update
            sudo apt-get install -y dotnet-sdk-9.0
        elif [ "$MACHINE" = "mac" ]; then
            brew install --cask dotnet-sdk
        fi
    fi

    exec >/dev/tty
    printf "✔\n"
}

install_mongodb() {
    printf "\t4. MongoDB "
    exec >/dev/null

    if [ "$MACHINE" = "win" ]; then
        if ! winget list --id MongoDB.Server > /dev/null 2>&1; then
            winget install -e --id MongoDB.Server
        fi
    elif [ "$MACHINE" = "mac" ]; then
        if ! brew list --formula | grep -q mongodb-community; then
            brew tap mongodb/brew
            brew install mongodb-community
        fi
    elif [ "$MACHINE" = "linux" ]; then
        if ! command -v mongod > /dev/null 2>&1; then
            sudo apt-get install -y gnupg
            curl -fsSL https://pgp.mongodb.com/server-7.0.asc | sudo gpg -o /usr/share/keyrings/mongodb-server-7.0.gpg --dearmor
            echo "deb [ arch=amd64,arm64 signed-by=/usr/share/keyrings/mongodb-server-7.0.gpg ] https://repo.mongodb.org/apt/ubuntu $(lsb_release -cs)/mongodb-org/7.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-7.0.list
            sudo apt-get update
            sudo apt-get install -y mongodb-org
        fi
    fi

    exec >/dev/tty
    printf "✔\n"
}

echo "Installing developer packages..."

install_vscode
install_node
install_dotnet
install_mongodb

install_interface() {
    printf "\t1. Install interface packages "
    exec >/dev/null

    cd interface
    npm i --force --silent

    exec >/dev/tty
    printf "✔\n"
}

install_engine() {
    printf "\t2. Restore engine packages "
    exec >/dev/null

    cd ../os
    dotnet restore -v q

    exec >/dev/tty
    printf "✔\n"
}

config_environment() {
    printf "\t3. Configure environment "
    exec >/dev/null

    cd ../mfe/source/interface/web
    # Populate environment variables

    exec >/dev/tty
    printf "✔\n"
}

install() {
    echo ""
    echo "Installing the system..."

    install_interface
    install_engine
    config_environment
}

install

cd "$SCRIPT_PATH"
cd ..

echo ""

echo "Setup complete."

code .