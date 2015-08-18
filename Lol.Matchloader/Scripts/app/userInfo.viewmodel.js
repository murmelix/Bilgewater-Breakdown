function UserInfoViewModel(app, name, dataModel) {
    var self = this;

    // Daten
    self.name = ko.observable(name);

    // Vorgänge
    self.logOff = function () {
        dataModel.logout().done(function () {
            app.navigateToLoggedOff();
        }).fail(function () {
            app.errors.push("Fehler bei der Abmeldung.");
        });
    };

    self.manage = function () {
        app.navigateToManage();
    };
}
