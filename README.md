
# 2FA CLI tool

### Download

[![GitHub Release](https://img.shields.io/github/v/release/jurakovic/2fa-cli)](https://github.com/jurakovic/2fa-cli/releases/latest)

### Build From Source

```
cd src
dotnet publish -c Release --self-contained
bin/Release/net8.0/linux-x64/publish/2fa -h
# and then mv to desired path
```

### Technologies

| Title | GitHub | NuGet |
|--|--|--|
| System.CommandLine | [dotnet/command-line-api](https://github.com/dotnet/command-line-api) | [System.CommandLine](https://www.nuget.org/packages/System.CommandLine) |
| bcrypt.net | [BcryptNet/bcrypt.net](https://github.com/BcryptNet/bcrypt.net) | [BCrypt.Net-Next](https://www.nuget.org/packages/BCrypt.Net-Next) |
| Otp.NET | [kspearrin/Otp.NET](https://github.com/kspearrin/Otp.NET) | [Otp.NET](https://www.nuget.org/packages/Otp.NET) |
| TextCopy | [CopyText/TextCopy](https://github.com/CopyText/TextCopy) | [TextCopy](https://www.nuget.org/packages/TextCopy) |

### Commands Overview

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
  add <service> <secret-key>  Adds a new 2FA entry
  get <service>               Retrieves the current OTP code for a specified service
  list                        Lists all 2FA entries
  remove, rm <service>        Removes an existing 2FA entry
```

```text
$ 2fa add -h
Description:
  Adds a new 2FA entry

Usage:
  2fa add <service> <secret-key> [options]

Arguments:
  <service>     The name of the organization or service provider
  <secret-key>  The base32-encoded secret key used to generate the OTP

Options:
  -t, --type <hotp|totp>  The type of OTP [default: totp]
  -d, --digits <6|8>      The number of digits in the OTP code [default: 6]
  -?, -h, --help          Show help and usage information
```

```text
$ 2fa get -h
Description:
  Retrieves the current OTP code for a specified service

Usage:
  2fa get <service> [options]

Arguments:
  <service>  The name of the organization or service provider

Options:
  -nc, --no-clipboard  Disable copying to clipboard [default: False]
  -?, -h, --help       Show help and usage information
```

```text
$ 2fa list -h
Description:
  Lists all 2FA entries

Usage:
  2fa list [options]

Options:
  -?, -h, --help  Show help and usage information
```

```text
$ 2fa remove -h
Description:
  Removes an existing 2FA entry

Usage:
  2fa remove <service> [options]

Arguments:
  <service>  The name of the organization or service provider

Options:
  -?, -h, --help  Show help and usage information
```
