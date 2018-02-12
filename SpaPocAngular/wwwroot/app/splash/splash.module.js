"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var platform_browser_1 = require("@angular/platform-browser");
var core_1 = require("@angular/core");
var material_1 = require("@angular/material");
var splash_component_1 = require("./splash.component");
var SplashModule = /** @class */ (function () {
    function SplashModule() {
    }
    SplashModule = __decorate([
        core_1.NgModule({
            declarations: [splash_component_1.SplashComponent],
            bootstrap: [splash_component_1.SplashComponent],
            imports: [
                platform_browser_1.BrowserModule,
                material_1.MdProgressSpinnerModule
            ],
            exports: [],
            providers: [],
        }),
        __metadata("design:paramtypes", [])
    ], SplashModule);
    return SplashModule;
}());
exports.SplashModule = SplashModule;
//# sourceMappingURL=splash.module.js.map