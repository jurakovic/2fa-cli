
# 2FA CLI tool

### Download

[![GitHub Release](https://img.shields.io/github/v/release/jurakovic/2fa-cli)](https://github.com/jurakovic/2fa-cli/releases/latest)

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
2fa add github nbswy3dpor3w6ztb
```

On first `add`, tool asks for password which is used for secret key encryption.

Getting the current OTP code for a specified entry:

```
2fa get github
```

On each `get`, tool asks for password for secret key decryption.

Password can also be set as `_2FA_CLI_PASSWORD` environment variable.

```
export _2FA_CLI_PASSWORD='s0M35tr0NgP4$$W0rD'
```

Listing all entries:

```
2fa list
```

Removing an existing entry:

```
2fa rm github
```

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
