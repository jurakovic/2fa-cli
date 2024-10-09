
# 2FA CLI

The **2FA CLI** is a simple command-line utility that allows users to securely add 2FA secrets and get OTP codes for various accounts directly from the terminal.

Entries are stored in the `$HOME` directory on Linux and the `%USERPROFILE%` directory on Windows in a `.2fa-cli.json` file.

All secret keys are **encrypted** using a key derived from a password, which is set when adding first entry. The password itself is securely hashed using **bcrypt**.

### Usage

```text
$ 2fa -h
Description:
  2FA CLI tool

Usage:
  2fa [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  add <service> <secret-key>  Adds a new entry
  get <service>               Gets the current OTP code for a specified entry
  list                        Lists all entries
  remove, rm <service>        Removes an existing entry
```

#### Examples

Adding a new entry:

```
$ 2fa add github nbswy3dpor3w6ztb
Entry 'github' added.
```

Getting the current OTP code for a specified entry:

```
$ 2fa get github
924313
```

Listing all entries:

```
$ 2fa list
github
```

Removing an existing entry:

```
$ 2fa rm github
Entry 'github' removed.
```

#### Password

Each time the `add` or `get` commands are used, the tool prompts for a password to encrypt or decrypt secret keys.

```
$ 2fa get github
Enter password:
```

For convenience, the password can also be provided using the `_2FA_CLI_PASSWORD` environment variable.

```bash
# Linux example
export _2FA_CLI_PASSWORD='s0M35tr0NgP4$$W0rD'
```

> Be aware of security implications. Environment variables can be accessed by any process running under the same user account.

#### .2fa-cli.json example

```json
{
  "PasswordHash": "$2a$11$gVUQlYe2WldoCo93JxdLe.tQgN.eD1QYcbxB69skesa4QYZjEmEJK",
  "Entries": [
    {
      "Service": "github",
      "SecretKey": "DmVSS8L27lGQ7cWsrWzEzg==:2KXQIEWTbrONknsO2wWxUMXLx6MMY41sx2FAkiIJhR8=",
      "Type": "totp",
      "Digits": 6
    }
  ]
}
```

### Download

[![GitHub Release](https://img.shields.io/github/v/release/jurakovic/2fa-cli)](https://github.com/jurakovic/2fa-cli/releases/latest)

### Build from source

0. Prerequisites

    * [.NET SDK](https://dotnet.microsoft.com/en-us/download)
    * [Git](https://git-scm.com/)

1. Clone the repository

    ```bash
    git clone https://github.com/jurakovic/2fa-cli.git
    cd 2fa-cli
    ```

3. Build (publish) with `dotnet`

    ```bash
    cd src
    dotnet publish -c Release --self-contained
    bin/Release/net8.0/linux-x64/publish/2fa -h
    # mv to desired path
    ```

### Technologies

| Title | GitHub | NuGet |
|--|--|--|
| System.CommandLine | [dotnet/command-line-api](https://github.com/dotnet/command-line-api) | [System.CommandLine](https://www.nuget.org/packages/System.CommandLine) |
| bcrypt.net | [BcryptNet/bcrypt.net](https://github.com/BcryptNet/bcrypt.net) | [BCrypt.Net-Next](https://www.nuget.org/packages/BCrypt.Net-Next) |
| Otp.NET | [kspearrin/Otp.NET](https://github.com/kspearrin/Otp.NET) | [Otp.NET](https://www.nuget.org/packages/Otp.NET) |
| TextCopy | [CopyText/TextCopy](https://github.com/CopyText/TextCopy) | [TextCopy](https://www.nuget.org/packages/TextCopy) |

### Disclaimer

This tool is intended for educational and personal use only. While every effort has been made to ensure the security and integrity of stored 2FA secrets, it is not recommended for use in high-security environments or as a replacement for professional-grade security solutions. Use this tool at your own risk. The author is not responsible for any data loss, security breaches, or other issues that may arise from the use of this software.
