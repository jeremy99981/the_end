# The End

The End est une petite application Windows portable destinée à préparer la transmission de fin de journée d’un magasin. Elle permet de noter ce qu’il reste à faire et les objectifs du lendemain, puis d’imprimer une fiche A4 claire avec la boîte de dialogue d’impression native de Windows.

## Utilisation

Générer `TheEnd.exe` avec le script de publication, le placer dans un dossier accessible (Bureau, Documents ou clé USB), puis le lancer par double-clic. Aucune installation, élévation UAC ou version préalable de .NET n’est nécessaire.

La date du jour est remplie automatiquement. Le champ équipier est facultatif. Le brouillon est sauvegardé automatiquement dans `%LOCALAPPDATA%\TheEnd\draft.json` et une restauration est proposée au prochain démarrage. Les raccourcis sont `Ctrl+P` pour imprimer et `Ctrl+Maj+Suppr` pour effacer après confirmation.

`Imprimer` ouvre le dialogue natif Windows : l’utilisateur choisit l’imprimante, les copies, l’orientation et toutes les options exposées par le pilote. Après une impression envoyée, l’effacement reste optionnel.

## Architecture

- `src/TheEnd.Core` : modèle de brouillon, date française, formatage et stockage local testables sans Windows.
- `src/TheEnd.App` : application WPF Windows, interface, aperçu et impression native via `System.Windows.Controls.PrintDialog`.
- `tests/TheEnd.Core.Tests` : tests xUnit de date, texte, accents, contenu vide, texte long et cycle sauvegarde/restauration/suppression.

WPF/.NET 8 a été retenu pour son intégration Windows et son accès direct à la pile d’impression native, tout en permettant un publish self-contained single-file. Le manifeste demande explicitement `asInvoker` : l’application ne demande aucun droit administrateur et n’écrit ni dans Program Files ni dans le registre système.

## Développement

Sur Windows avec le SDK .NET 8 :

```powershell
dotnet restore TheEnd.sln
dotnet test TheEnd.sln --configuration Release
dotnet run --project src/TheEnd.App/TheEnd.App.csproj
```

Le build portable est :

```powershell
dotnet publish src/TheEnd.App/TheEnd.App.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o artifacts/TheEnd-Windows-x64
```

## Générer l’exécutable Windows depuis macOS

Le script `scripts/build-windows.sh` utilise le ciblage Windows du SDK .NET pour produire un exécutable x64 autonome et single-file, sans nécessiter Windows sur la machine de développement :

```bash
./scripts/build-windows.sh
```

Le résultat est `artifacts/TheEnd-Windows-x64/TheEnd.exe`. Le dossier `artifacts/` est ignoré par Git. Le fichier obtenu peut être copié sur le Bureau, dans Documents, sur une clé USB ou dans un dossier partagé Windows.

## Sécurité et données

The End ne transmet aucune donnée sur Internet. Le seul fichier écrit par l’application est le brouillon dans le profil utilisateur local. Aucun service, pilote, clé de registre système ou installation administrateur n’est utilisé.

## Limitation de validation

La compilation WPF et l’ouverture réelle du dialogue d’impression nécessitent Windows. Elles sont donc validées par le runner GitHub Actions Windows ; les tests du cœur restent exécutables sur macOS/Linux.
