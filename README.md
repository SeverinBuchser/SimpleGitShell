# Simple Git Shell

![License][license-badge] ![GitHub Release][release-badge] ![GitHub Issues][issues-badge] ![GitHub Pull Requests][pr-badge] ![Code Coverage][coverage-badge]

Simple Git Shell is a .NET-based project that provides a set of git-shell executables for managing groups, repositories, and SSH access on a Git server. These executables allow administrators to perform essential Git server management tasks from the command line. They are designed to be run using the `ssh` syntax, making it easy to integrate them into your Git server infrastructure.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
  - [Prerequisites](#prerequisites)
  - [Testing](#testing)
  - [Building](#building)
  - [Publishing](#publishing)
- [License](#license)

## Features

To use Simple Git Shell, please refer to the detailed usage instructions provided in the [Usage][usage] section. The following features are available:

### 1. Group

- **List Groups**: List all existing groups on the Git server.
- **Create Group**: Create a new group on the Git server.
- **Remove Group**: Delete an existing group from the Git server.

### 2. Repositories

- **List Repositories**: List all Git projects (repositories) on the Git server.
- **Create Repository**: Create a new Git project (repository) on the Git server.
- **Remove Repository**: Delete an existing Git project (repository) from the Git server.

### 3. SSH
#### 1. User

- **Add SSH Key**: Add an SSH key for a user to allow secure access.
- **Remove SSH Key**: Remove an SSH key from a user's authorized keys list.

## Installation

### Install with Install Script

To simplify the installation process, you can use the provided [install.sh][install-script] script. This script automates the installation of the Git Shell Executables on your remote Git server. Follow these steps to use the script:

1. **Download Executables**: Obtain the Git Shell Executables by downloading the latest release from the [Releases][releases] page on the project's GitHub repository. You will find a zip file containing the executables. Extract the executables from the zip file.

2. **Download Installation Script**: Additionally, download the [install.sh][install-script] script from the same [Releases][releases] page. This script is essential for the installation process.

3. **Execute the Installation Script**: Open your terminal and navigate to the directory where you downloaded the [install.sh][install-script] script. Run the script using the following command:

    ```bash
    bash install.sh
    ```

4. **Follow Prompts**: The script will prompt you for the necessary information to complete the installation. Follow the prompts:

    - When prompted, provide the local path to the directory where you extracted the Git Shell Executables.

    - When prompted, provide the remote server's SSH access in the format `user@server`.

    - When prompted, provide the path to the home directory of the `git` user on your remote server.

5. **Complete Installation**: The script will copy the Git Shell Executables to your remote server and configure them for use.

Your Git Shell Executables are now installed and ready to use on your remote Git server. You can access and manage your Git server infrastructure using the provided command-line executables.

### Manual Installation

If you prefer to perform the installation manually, you can follow these steps:

1. **Download Executables**: Obtain the Git Shell Executables by downloading the latest release from the [Releases][releases] page on the project's GitHub repository. You will find a zip file containing the executables.

2. **Extract Executables**: Extract the contents of the zip file to your local machine.

3. **Copy Executables**: Copy the extracted executables to your Git server. If you have `git-shell` enabled, use the provided commands below. Replace placeholders with actual values:

    ```bash
    scp -rq /path/to/extracted-folder <user-with-sudo>@<your-git-server>:extracted-folder
    sudo cp -rf extracted-folder/* /path/to/git-shell-commands
    sudo chown -R git:git /path/to/git-shell-commands/
    ```

4. **Configure `git` User's Shell**: Configure the `git` user's shell to use `git-shell` by running the following command on the git-server:

    ```bash
    sudo chsh -s $(which git-shell) git
    ```

### **Verify Installation**

After the installation, it's important to verify that the Git Shell Executables are correctly installed and working. You can do this by connecting to your Git server using the `ssh` syntax and running a simple command. For example, you can check the list of groups by using the following command:

```bash
ssh git@<your-git-server> group list
```

If the command returns a list of groups, it means that the installation was successful, and the Git Shell Executables are functioning properly. You can similarly check the functionality of other commands provided by the executables.

For more information on using the Git Shell Executables, refer to the [Usage][usage] section in this README.

### Troubleshooting

If the executables are not automatically marked as executable after the installation, you may need to make them executable manually using the `chmod` command on the git-server:

```bash
sudo chmod +x /path/to/git-shell-commands/*
```

This command ensures that all the executables in the directory are made executable.

[releases]: https://github.com/SeverinBuchser/SimpleGitShell/releases/latest
[install-script]: https://github.com/SeverinBuchser/SimpleGitShell/releases/latest/download/install.sh

## Usage

You can use the executables by connecting to your Git server using the `ssh` syntax and specifying the desired command. All commands support a `--help` option for displaying command-specific help, e.g:

```bash
group list --help
```

or

```bash
repo --help
```

### Group Commands:

```bash
group <COMMAND>
```
#### List all groups:

```bash
group list [-b|--base-group]
```

#### Create a new group:

```bash
group create <group> [-b|--base-group]
```

#### Remove a group:

```bash
group remove <group> [-b|--base-group]
```

### Repository Commands:

```bash
repo <COMMAND>
```

#### List all repositories:

```bash
repo list [-g|--group]
```

#### Create a new repository:

```bash
repo create <repository> [-g|--group]
```

#### Remove a repository:

```bash
repo remove <repository> [-g|--group]
```

### SSH Commands:

```bash
ssh <COMMAND>
```

#### Manage SSH access for users:

```bash
ssh user <COMMAND>
```

##### Add a user by adding an SSH key:

```bash
ssh user add <public-key>
```

Where the

 `<public-key>` has the following format:

```bash
"ssh-rsa YOUR_SSH_PUBLIC_KEY"
```

##### Remove a user by removing an SSH key:

```bash
ssh user remove <public-key>
```

Where the `<public-key>` has the following format:

```bash
"ssh-rsa YOUR_SSH_PUBLIC_KEY"
```

## Contributing

We welcome contributions from the open-source community. If you would like to contribute to the Git Server Shell Executables project, please follow these guidelines:

### Prerequisites

Before contributing, ensure you have the following prerequisites:

- **.NET SDK**: Install .NET if you haven't already. You can download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download).

### Testing

Before submitting a pull request, ensure that you have tested the project with .NET to validate the changes:

1. Navigate to the project directory and run the tests using the following command:

```bash
dotnet test
```

This will execute the test suite to verify that your changes do not introduce regressions and conform to the project's coding standards.

### Building

If you make changes to the source code, you can build the project with the following command:

```bash
dotnet build
```

This will compile the project, and you can then execute the generated executables as described in the [Usage][usage] section.

### Publishing

To publish the project and create executables, use the following command:

```bash
dotnet publish
```

This command will create the executables and place them into the `bin` directory of the root of the project.

Please ensure that you follow the project's code of conduct and licensing terms.

## License

This project is licensed under the [MIT License][license].

[license]: https://github.com/SeverinBuchser/SimpleGitShell/blob/develop/LICENSE
[license-badge]: https://img.shields.io/github/license/SeverinBuchser/SimpleGitShell
[release-badge]: https://img.shields.io/github/v/release/SeverinBuchser/SimpleGitShell
[issues-badge]: https://img.shields.io/github/issues/SeverinBuchser/SimpleGitShell
[pr-badge]: https://img.shields.io/github/issues-pr/SeverinBuchser/SimpleGitShell
[coverage-badge]: https://img.shields.io/codecov/c/github/SeverinBuchser/SimpleGitShell