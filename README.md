
# 2FA CLI tool

### Download

### Technologies

### Credits

### Commands Overview

```bash
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

```bash
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

```bash
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

```bash
$ 2fa list -h
Description:
  Lists all 2FA entries

Usage:
  2fa list [options]

Options:
  -?, -h, --help  Show help and usage information
```

```bash
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
