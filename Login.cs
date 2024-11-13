using System;

public class Login{
    public Login(){
        Session.gI().Connect("localhost", 1609);
        Session.gI().SetHandler(Controller.gI());
        // Service.gI().login("duy", "asfafd", "1.8.8");
        Service.gI().register("duy", "asfafd");
    }
}