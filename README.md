
# 2FA-CLI

2fa command line tool

Usage:

```
  2fa [command] [options]
```

Commands:

```
  add <service> <secret-key>  Adds a new 2FA entry to the storage
  get <service>               Retrieves the current OTP code for a specified 2FA entry
  list                        Lists all stored 2FA entries
  remove, rm <service>        Removes an existing 2FA entry from storage
```

Options:

```
  --version       Show version information
  -?, -h, --help  Show help and usage information
```
