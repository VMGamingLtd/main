### Python virtual environmeny

Open powershell terminal in project root and follow the steps below.

Install python virtual environment:

```
> python -m venv .venv
```

Source in python virtual environment:

```
> .venv\Scripts\Activate.ps1
```

Install packages after sourcing in python virtual environment:

```
(.venv) > pip install -r requirements.txt
```

Set python library path:

```
(.venv) > $Env:PYTHONPATH=‘.\’
```

### Create new repository

Set gaow.git repository on remote server:

```
> su - git
> cd repos
> mkdir  gaow.git
> cd gaow.git
> git init --bare
```

Execute following on local server inside gaow project folder:

```
> cd gaow
> git init
> git add .
> git commit -m 'Initial commit'
> git remote add origin  git@212.24.97.13:repos/gaow.git
> git push origin -u main
```

