using System;

public class Login{
    public Login(){
        Session.gI().Connect("localhost", 1609);
        Session.gI().SetHandler(Controller.gI());
        Service.gI().login("duy", "sdfaf", "1.8.8");
        Service.gI().login("duy", "sdfaf", "1.8.8");
    }
}