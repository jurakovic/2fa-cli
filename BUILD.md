
#### Step 0

Windows requires [Git Bash](https://gitforwindows.org), or more specifically [MSYS2](https://www.msys2.org).

```
# Windows/Linux
version="1.0.0"

# Show help
./publish.sh -h
```

#### Step 1

```bash
# Windows
./publish.sh publish -v $version -a win-x64

# Linux
./publish.sh publish -v $version -a linux-x64
```

#### Step 2

```bash
# Windows
./publish.sh package -v $version -a win-x64 -u true
./publish.sh package -v $version -a linux-x64 -u true
```

#### Step 3

```bash
# Windows
./publish.sh release -v $version
```
