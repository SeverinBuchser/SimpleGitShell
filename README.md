# Simple Git Shell

[![![License](https://img.shields.io/github/license/SeverinBuchser/SimpleGitShell)](LICENSE)

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

To use Simple Git Shell, please refer to the detailed usage instructions provided in the [Usage](#usage) section. The following features are available:

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

1. **Download Executables**: Download the latest release of Git Server Shell Executables from the [Releases](https://github.com/SeverinBuchser/SimpleGitShell/releases) section on GitHub. You will find a zip file containing the executables.

2. **Extract and Copy**: Extract the contents of the zip file to your local machine. Copy the extracted executables to your Git server. If you already enabled `git-shell` you need to do this with another user who has `sudo` access on the server. If not, you can use the git user.
   
   1. **Copy Executables to the git-server:**
      
	```bash
	scp -rq /path/to/extracted-folder <user-with-sudo>@<your-git-server>:extracted-folder
	```
   
   2. **Copy the Executables to the `git-shell-commands` directory on the git-server:**
      
	```bash
	sudo cp -rf extracted-folder/* /path/to/git-shell-commands
	```
   
   3.  **Remove the `extracted-folder` from the git-server:**
      
	```bash
	sudo rm -rf extracted-folder
	```
   
   4.  **Remove the `extracted-folder` from your local machine:**
      
	```bash
	rm -rf /path/to/extracted-folder
	```

4. **Set Ownership**: Set the owner of the executables to the `git` user recursively by running the following command on the git-server:

    ```bash
    sudo chown -R git:git /path/to/git-shell-commands/
    ```

4. **Configure `git` User's Shell**: Configure the `git` user's shell to use `git-shell` by running the following command on the git-server:

    ```bash
    sudo chsh -s $(which git-shell) git
    ```

   For more information on configuring `git-shell`, refer to the [git-shell documentation](https://git-scm.com/docs/git-shell) provided by the official Git documentation.

5. **Checking installation:** You should now be able to log into the git-server via ssh and be able to execute the scripts. To verify you can run the following command on your local machine:
   
    ```bash
	ssh git@<your-git-server> group --version
    ```
    
    This is also the intended usage of the commands.

### Troubleshooting

If the executables are not automatically marked as executable after the installation, you may need to make them executable manually using the `chmod` command on the git-server:

```bash
sudo chmod +x /path/to/git-shell-commands/*
```

This command ensures that all the executables in the directory are made executable.

Please ensure that you follow the project's code of conduct and licensing terms.

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

Where the `<public-key>` has the following format:

```shell
"ssh-rsa YOUR_SSH_PUBLIC_KEY"
```

##### Remove a user by removing an SSH key:

```bash
ssh user remove <public-key>
``````

Where the `<public-key>` has the following format:

```shell
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

This will compile the project, and you can then execute the generated executables as described in the [Usage](#usage) section.

### Publishing

To publish the project and create executables, use the following command:

```bash
dotnet publish
```

This command will create the executables and place them into the `bin` directory of the root of the project.

Please ensure that you follow the project's code of conduct and licensing terms.

## License

This project is licensed under the [MIT License](https://github.com/SeverinBuchser/SimpleGitShell/blob/develop/LICENSE).