#### Install python virtual environment

In powershell:

```
> python -m venv .venv
```

#### Source in python virtual environment

In powershell:

```
> .venv\Scripts\Activate.ps1
```

#### Install packages

After sourcing in python virtual environment:

```
(.venv) > pip install -r requirements.txt
```

```
(.venv) > $Env:PYTHONPATH=‘.\’
```

