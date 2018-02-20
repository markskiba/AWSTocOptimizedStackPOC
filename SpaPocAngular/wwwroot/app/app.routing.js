"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var routes = [
    //{ path: '', component: SplashComponent },
    { path: 'app', loadChildren: 'app/home/home.module#HomeModule' }
];
exports.routing = router_1.RouterModule.forRoot(routes);
//# sourceMappingURL=app.routing.js.map