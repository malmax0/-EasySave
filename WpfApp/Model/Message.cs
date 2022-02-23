namespace WpfApp.Model
{


    static class Message
    {

        public static string[,] MessageTable =
        {
             {// 0
                "\tINCORRECT ENTRY ! Please enter a number present in the proposals of the above list.\n",
                 "\tENTREE INCORRECTE ! Entrez un nombre présent dans les propositions de la liste ci-dessus s'il vous plaît.\n"
            },

            {
                "Select a language",
                "Sélectionnez une langue"
            },

            {
                "------------- MAIN MENU -------------\nSelect an action\n1 - Add a backup task\n2 - Delete a backup task\n3 - Show a backup task\n4 - Launch a backup task\n5 - Change langage\n0 - Leave the application\n",
                "------------- MENU PRINCIPAL -------------\nSélectionnez une action\n1 - Ajouter une tâche de sauvegarde\n2 - Supprimer une tâche de sauvegarde\n3 - Afficher une tâche de sauvegarde\n4 - Lancer une tâche de sauvegarde\n5 - Changer la langue\n0 - Quitter l'application\n"
            },

            {// 3
                "---> ADD\n",
                "---> AJOUT\n"
            },

            {
                "\tADDITION IMPOSSIBLE ! Five tasks are already added ...\n",
                "\tAJOUT IMPOSSIBLE ! Cinq tâches ont déjà été ajoutées …\n"
            },

            {// 5
                "Specify the parameters to add a task :\n",
                "Précisez les paramètres pour ajouter une tâche :\n"
            },


            {
                "- Backup Type : \n\tBy default, the backup type is ''full''\n\tEnter differential to change the backup type.\n",
                "- Type de sauvegarde : \n\tPar défaut, le type de sauvegarde est ''full''\n\tEntrer ''differential'' pour changer le type de backup.\n"
            },

            {
                "\tADDITION IMPOSSIBLE ! Incorrect source path(s) ... Please, try again.\n",
                "\tIMPOSSIBLE ADDITION ! Chemin(s) source incorrect(s) … Réessayer, s'il vous plaît.\n"
            },

            {
                "\tADDITION IMPOSSIBLE ! Incorrect destination path(s) ... Please, try again.\n",
                "\tIMPOSSIBLE ADDITION ! Chemin(s) de destination incorrect(s) … Réessayer, s'il vous plaît.\n"
            },

            {
                "---> SHOW\nEnter the number corresponding to the task number you want to show (between 1 and 5)\nOR Enter 0 to come back to the main menu\n",
                "---> AFFICHER\nEntrez un numéro correspondant à la tâche que vous souhaitez afficher (entre 1 et 5)\nOU Entrez 0 pour revenir au menu principal\n"
            },

            {//10
                "Task Name : ",
                "Nom de la tâche : "
            },

            {
                "Source Foler Path : ",
                "Chemin du Dossier Source : "
            },

            {//12
                "Target Folder Path : ",
                "Chemin du Dossier Destination : "
            },

            {//13
                "Backup Type : ",
                "Type de sauvegarde : "
            },

            {//14
                "- State : ",
                "- Etat : "
            },

            {//15
                "- Number of Files To Copy : ",
                "- Nombre de fichiers à copier : "
            },

            {//16
                "- Number of Files Left To Do : ",
                "- Nombre de fichiers restants : "
            },

            {//17
                "- Progress : ",
                "- Progression : "
            },

            {//18
                "- Start Transfert Time : ",
                "- Début de transfert : "
            },

            {//19
                "- End Transfert Time : ",
                "- Fin de transfert : "
            },

            {
                "\tPARAMETRE INCONNU \n",
                "\tSETTING UNKNOWN \n"
            },

            {//21
                "---> DELETE \nEnter the number corresponding to the task number you want to delete \nOR Enter 0 to come back to the main menu \n",
                "---> SUPPRIMER \nEntrez un numéro correspondant à la tâche que vous souhaitez supprimer \nOU Entrez 0 pour revenir au menu principal \n"
            },

            {
                "You will delete this backup task … Are you sure ? \n1 - Yes. \n0 - Cancel. \n",
                "Vous allez supprimer cette tâche de sauvegarde …  Etes-vous sûr ? \n1 - Oui. \n2 - Annuler. \n"
            },

            {//23
                "\tDELETION IMPOSSIBLE - Please select a task !\n",
                "\tSUPPRESSION IMPOSSIBLE - Veuillez sélectionner une tâche !\n"
            },

            {
                "---> LAUNCH \nEnter the numbers corresponding to the tasks number you want to launch. Use ';' to separate numbers. \nOR Enter 0 to come back to the main menu \n",
                "---> LANCER \nEntrez les numéros correspondants aux tâches que vous souhaitez lancer. Utilisez ';' pour séparer les numéros. \nOU Entrez 0 pour revenir au menu principal \n"
            },


            {
                "You will launch this backup task. Are you sure ? \n1 - Yes. \n0 - Cancel. \n",
                "Vous allez lancer cette tâche de sauvegarde. Etes-vous sûr ? \n1 - Oui. \n2 - Annuler. \n"
            },

             {
                "\tLAUNCH IMPOSSIBLE - Please select a task !\n",
                "\tLANCEMENT IMPOSSIBLE - Veuillez sélectionner une tâche !\n"
            },

            {//27
                "IMPOSSIBLE ! No backup exists yet you should start by creating one. \n",
                "IMPOSSIBLE ! Aucune sauvegarde n'existe encore vous devriez commencer par en créer une."
            },

            {//28
                "Task added !\n",
                "Tâche ajoutée !\n"
            },

            {//29
                "- Id : ",
                "- Id : "
            },

            {
                "Task delete succesfully !\n",
                "Tâche supprimée avec succès !\n"
            },

            {//31
                "Copy made succesfully !\n",
                "Copie effectuée avec succès !\n"
            },

            {//32
                "ALREADY EXISTING ! Ingoring it \n",
                "EXISTE DEJA ! Fichier ignoré \n",
            },

            {//33
                "ALREADY EXISTING ! Ingoring it \n",
                "EXISTE DEJA ! Fichier ignoré \n",
            },

            {//34
                "Search by name ...",
                "Recherche par nom ..."
            },

            {//35
                "State file path",
                "Chemin du fichier state"
            },

            {
                "Log file path",
                "Chemin du fichier de Log"
            },

            {//37
                "CryptoSoft Software path",
                "Chemin du logiciel CrytoSoft"
            },

            {
                "Log file extension",
                "Extension du fichier de log"
            },

            {//39
                "Extensions to encrypt (ex format: .txt, .doc)",
                "Extensions à encrypter (ex format: .txt, .doc)"
            },

            {
                "Modify",
                "Modifier"
            },

            {
                "Backup Tasks",
                "Tâches de sauvegarde"
            },

            {//42
                "Settings",
                "Paramètres"
            },

            {
                "Business software",
                "Logiciel métier"
            }
            ,
            {// 44
                "Business software detected",
                "Logiciel metier detecté"
            },
            {
                "Priority files (ex format: .txt, .doc)",
                "Fichiers prioritaires (ex format: .txt, .doc)"
            },
            {
                "File size limit (in Kilobytes)",
                "Taille limite d'un fichier (en Kilo octets)"
            },

            {
                "Another instance of EasySave is running, we cannot open a new instance",
                "Une autre instance d'EasySave est en cours d'execution, nous ne pouvons pas ouvrir une nouvelle instance du logiciel"
            }

        };
    }


}