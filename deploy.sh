# Run Tests
dotnet test --nologo --verbosity quiet
if [ $? != 0 ]
then
    echo "Tests failed. Aborting Deploy."
    exit 0
fi

# Publishing
echo "Publishing Commands..."
dotnet publish -r linux-arm64 --self-contained --nologo --verbosity quiet


# Copying
echo "Copying commands to remote..."
scp -rq bin/publish/commands ubuntu@192.168.1.10:git-shell-commands
ssh ubuntu@192.168.1.10 sudo -S cp -rf git-shell-commands/* /var/git/git-shell-commands
ssh ubuntu@192.168.1.10 rm -rf git-shell-commands