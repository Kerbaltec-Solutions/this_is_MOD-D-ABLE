public class ui{
    public static void printIntro(){
        Console.Clear();
        string outp="";
        outp+="___ _  _ _ ____    _ ____ \n";
        outp+=" |  |__| | [__     | [__  \n";
        outp+=" |  |  | | ___]    | ___] \n";
        outp+="\n\n";
        outp+="'##::::'##::'#######::'######::::::'###:':::######:'###::::::'###::::'########::'##:::::::'########:\n";
        outp+=" ###::'###:'##.... ##: ##.. ###:::'##:::: ###.. ##:.. ##::::'## ##::: ##.... ##: ##::::::: ##.....::\n";
        outp+=" ####'####: ##:::: ##: ##:::: ##:'##:::: ##:::: ##:::: ##::'##:. ##:: ##:::: ##: ##::::::: ##:::::::\n";
        outp+=" ## ### ##: ##:::: ##: ##:::: ##: ##:::: ##:::: ##:::: ##:'##:::. ##: ########:: ##::::::: ######:::\n";
        outp+=" ##. #: ##: ##:::: ##: ##:::: ##: ##:::: ##:::: ##:::: ##: #########: ##.... ##: ##::::::: ##...::::\n";
        outp+=" ##:.:: ##: ##:::: ##: ##:: ###::. ##:::: ###:: ##::: ##:: ##.... ##: ##:::: ##: ##::::::: ##:::::::\n";
        outp+=" ##:::: ##:. #######:: ######:::::. ###:::. ######: ###::: ##:::: ##: ########:: ########: ########:\n";
        outp+="..:::::..:::.......:::........:::::...::........:::...::::..:::::..::........:::........::........::\n";
        outp+="\n\nVersion: "+TIM.version+"\n";
        outp+="\n press ENTER to enter and exit input mode\n"
        outp+=" in input mode enter \"sys:help()\" for tutorial\n"
        outp+=" press any key to start";
        Console.WriteLine(outp);
    }
    public static void printOutro(){
        string outp="\nThank you for playing.\n\n";
        outp+="Main game by Björn Schnabel and Hannah Kabisch\n";
        outp+="Thank you to everyone contributing content.\n";
        outp+="Copyright (c) 2023 Kerbaltec-Solutions\n";
        outp+="All gamefiles are shared under MIT License\n\n";
        outp+="Version: "+ TIM.version+"\n";
        outp+="\n press any key to exit";
        Console.WriteLine(outp);
    }
}