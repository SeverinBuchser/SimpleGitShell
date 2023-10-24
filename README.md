# Simple Git Shell

![License][license-badge] ![GitHub Release][release-badge] ![GitHub Issues][issues-badge] ![GitHub Pull Requests][pr-badge] ![Code Coverage][coverage-badge]

Simple Git Shell is a .NET-based project that provides a custom Git shell for managing groups, repositories, and SSH access on a Git server. This Git shell is designed to be used as the default shell for a Git user on the server, allowing administrators to perform essential Git server management tasks directly from the command line.

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

To use the Simple Git Shell, please refer to the detailed usage instructions provided in the [Usage](#usage) section. The following features are available:

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

To simplify the installation process, you can use the provided [install.sh][install-script] script. This script automates the installation of the Simple Git Shell on your remote Git server. Follow these steps to use the script:

1. **Download the Shell Executable and install script**: Obtain the Simple Git Shell script and the [install.sh][install-script] script by downloading them from the latest release on the [Releases][releases] page of the project's GitHub repository.

2. **Execute the Installation Script**: Open your terminal and navigate to the directory where you downloaded the [install.sh][install-script] script. Run the script using the following command:

    ```bash
    bash install.sh
    ```

3. **Follow Prompts**: The script will prompt you for the local path to the Simple Git Shell executable and the destination SSH address (in the format user@host). Follow the prompts without specifying where to store the shell script.

Your Simple Git Shell is now installed to `/usr/bin/simple-git-shell` on your remote Git server and ready to use as the default shell for Git users.

### Manual Installation

If you prefer to perform the installation manually, you can follow these steps (**you will need to login to the git-server with a `sudoer` user**):

1. **Download the Shell Executable**: Obtain the Simple Git Shell script by downloading the latest release from the [Releases][releases] page on the project's GitHub repository. You will find the executable in the release assets.

2. **Copy the Shell Script to the Server**: Use the following command to copy the downloaded shell script to your Git server:

	```bash
	scp -q path/to/simple-git-shell user@host:simple-git-shell
	```

	Replace "path/to/simple-git-shell" with the path to the downloaded executable and the "user@host" with the credentials of your git-server. The `user` should not be the git-user but a `sudoer`.

3. **SSH into the Server**: After copying the shell script, SSH into your Git server using the following command:

	```bash
	ssh user@host
	```

4. **Move the Shell Script**: Once connected to the server, run the following command to move the shell script to the `/usr/bin` directory:

	```bash
	sudo mv -f simple-git-shell "/usr/bin/simple-git-shell"
	```

5. **Change Ownership**: Run the following command to change the ownership of the Simple Git Shell script to `root`:

	```bash
	sudo chown root:root "/usr/bin/simple-git-shell"
	```

	This step is just in case there are some owner issues.

6. **Set as Default Shell for the git user**: Configure the Git user's default shell to use the Simple Git Shell script. Run the following command on the Git server:

    ```bash
    sudo chsh -s /path/to/simple-git-shell-script git
    ```

Your Simple Git Shell is now installed, configured as the default shell for Git users, and ready for use on your remote Git server.

### **Verify Installation**

After the installation, it's important to verify that the Simple Git Shell is correctly installed and working. You can do this by running the following command:

```bash
ssh user@host -- --version
```

If you see a version number being returned, the installation is successful. 🥳

For more information on using the Simple Git Shell, refer to the [Usage](#usage) section in this README.

### Troubleshooting

If the Simple Git Shell script is not automatically marked as executable after the installation, you may need to make it executable manually using the `chmod` command on the Git server:

```bash
sudo chmod +x /usr/bin/simple-git-shell
```

This command ensures that the shell script is made executable.

[releases]: https://github.com/SeverinBuchser/SimpleGitShell/releases/latest
[install-script]: https://github.com/SeverinBuchser/SimpleGitShell/releases/latest/download/install.sh

## Usage

The Simple Git Shell is intended to be used as the default shell for Git users on your Git server. Once configured, Git users can log in using SSH and directly interact with the Simple Git Shell to manage groups, repositories, and SSH access, e.g:

```bash
ssh user@host group list
```

### Simple Git Shell Commands

The Simple Git Shell provides a set of commands to manage your Git server. Users can run these commands with various options to perform specific tasks. To get help on a specific command, you can use the `--help` option, e.g:

```bash
simple-git-shell group list --help
```

#### Group Commands:

```bash
simple-git-shell group <COMMAND>
```
#### List all groups:

```bash
simple-git-shell group list [-b|--base-group]
```

#### Create a new group:

```bash
simple-git-shell group create <group> [-b|--base-group]
```

#### Remove a group:

```bash
simple-git-shell group remove <group> [-b|--base-group]
```

#### Repository Commands:

```bash
simple-git-shell repo <COMMAND>
```

#### List all repositories:

```bash
simple-git-shell repo list [-g|--group]
```

#### Create a new repository:

```bash
simple-git-shell repo create <repository> [-g|--group]
```

#### Remove a repository:

```bash
simple-git-shell repo remove <repository> [-g|--group]
```

#### SSH Commands:

```bash
simple-git-shell ssh <COMMAND>
```

#### Manage SSH access for users:

```bash
simple-git-shell ssh user <COMMAND>
```

##### Add a user by adding an SSH key:

```bash
simple-git-shell ssh user add <public-key>
```

Where the `<public-key>` has the following format:

```bash
"ssh-rsa YOUR_SSH_PUBLIC_KEY"
```

##### Remove a user by removing an SSH key:

```bash
simple-git-shell ssh user remove <public-key>
```

Where the `<public-key>` has the following format:

```bash
"ssh-rsa YOUR_SSH_PUBLIC_KEY"
```

## Contributing

We welcome contributions from the open-source community. If you would like to contribute to the Simple Git Shell project, please follow these guidelines:

### Prerequisites

Before contributing, ensure you have the following prerequisites:

- **.NET SDK**: Install .NET if you haven't already. You can download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download).

### Testing

Before submitting a pull request, ensure that you have tested the project to validate your changes. You have two options for testing:

1. **Manual Testing**: Navigate to the project directory and run the tests using the following command:

     ```bash
     dotnet test
     ```

   This will execute the test suite to verify that your changes do not introduce regressions and conform to the project's coding standards.

2. **Automated Testing Script**: Alternatively, you can use the provided testing script located at `.scripts/test.sh` to run the tests:

     ```bash
     .scripts/test.sh [options]
     ```

	Options:
	- `-c, --coverage`: Perform coverage analysis. Cannot be used with the watch option.
	- `-w, --watch`: Start watching for changes.
	- `-d, --debug`: Enable debugging mode.

	This script automates the testing process, making it a convenient way to ensure that your changes do not introduce regressions. It can be used as an alternative to manual testing.

Both options help ensure that your contributions conform to the project's standards and do not introduce issues.

### Building

If you make changes to the source code, you can build the project with the following command:

```bash
dotnet build
```

This will compile the project, and you can then execute the generated shell script as described in the [Usage](#usage) section.

### Publishing

To publish the project and create the shell script, use the following command:

```bash
dotnet publish
```

This command will generate the shell script, and you can deploy it to your Git server.

Please ensure that you follow the project's code of conduct and licensing terms.

### Deployment

If you are responsible for deploying the Simple Git Shell to a Git server, you can use the provided deployment script located at `.scripts/deploy.sh`. To deploy the Simple Git Shell, follow these steps:

1. Open a terminal and navigate to your project directory.
2. Run the deployment script using the following command:

   ```bash
   .scripts/deploy.sh -d user@host
   ```

   Replace `user@host` with the destination SSH address of your Git server.

This deployment script automates the process of deploying the Simple Git Shell to your remote Git server, making it a convenient way to ensure a smooth deployment.

If you have any further updates or questions, please let me know.

## License

This project is licensed under the [MIT License][license].

[license]: https://github.com/SeverinBuchser/SimpleGitShell/blob/develop/LICENSE
[license-badge]: https://img.shields.io/github/license/SeverinBuchser/SimpleGitShell
[release-badge]: https://img.shields.io/github/v/release/SeverinBuchser/SimpleGitShell
[issues-badge]: https://img.shields.io/github/issues/SeverinBuchser/SimpleGitShell
[pr-badge]: https://img.shields.io/github/issues-pr/SeverinBuchser/SimpleGitShell
[coverage-badge]: https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/SeverinBuchser/cf21e101721df17339c0a05c23f4d7c2/raw/code-coverage.yml
