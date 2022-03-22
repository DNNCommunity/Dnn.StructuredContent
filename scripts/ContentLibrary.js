'use strict';
define(
  function () {

    return {
      init: function (wrapper, util, params, callback) {
        //console.log("init");
        // load angular and bootstrap first
        var siteRoot = util.sf.getSiteRoot();
        require(
          [
            "https://ajax.googleapis.com/ajax/libs/angularjs/1.7.2/angular.min.js",
            siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/bootstrap/js/bootstrap.bundle.min.js",
            siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/pluralize/pluralize.js"
          ],
          function () {
            //console.log("angular and bootstrap loaded");
            // next load all the items that depend on angular
            require([
              "https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-messages.min.js",
              "https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-animate.min.js",
              "https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-sanitize.min.js",
              "https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-cookies.min.js",
              siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-toastr/angular-toastr.tpls.min.js",
              siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ui.bootstrap/ui-bootstrap-tpls-2.5.0.min.js",
              siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/angular-drap-and-drop-lists/angular-drap-and-drop-lists.min.js",
              siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ngMask/ngMask.js",
              siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ace/ace.js",
              siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/app.js"


            ], function () {
              //console.log("app loaded");
              //next load all the things that depends on the app                            
              require([
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/plugins/ace/ui-ace.js",

                // content-field-type-options
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/boolean/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/boolean/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/choice/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/choice/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/date-time/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/date-time/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/email/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/email/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/number/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/number/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/phone-number/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/phone-number/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/static/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/static/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/text/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/text/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/url/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-type-options/url/controller.js",


                // content-field-types
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/service.js",

                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/boolean/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/boolean/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/choice/edit/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/date-time/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/date-time/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/email/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/email/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/number/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/number/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/phone-number/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/phone-number/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/list/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/list/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/add/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-multiple/delete/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-single/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/related-content-single/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/static/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/static/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/text/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/text/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-field-types/url/controller.js",

                // content-types
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/service.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/list/directive.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/list/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/add/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/edit/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-types/delete/controller.js",


                // content-fields
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/service.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/edit/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-fields/delete/controller.js",

                // content-item
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/service.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/list/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/edit/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/content-items/delete/controller.js",

                // relationship
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/service.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/edit/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/relationships/delete/controller.js",

                // revision
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/service.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/detail/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/revisions/list/controller.js",

                // visualizer-template
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/service.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/list/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/edit/controller.js",
                siteRoot + "DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.StructuredContent/app/visualizer-templates/delete/controller.js"
              ], function () {
                //console.log('everything is loaded');
                // manually bootstrap the angular app 

                angular.element(function () {
                  angular.bootstrap(document.getElementById('StructuredContent'), ['StructuredContent']);
                });
              });
            });
          }
        );
      },

      initMobile: function (wrapper, util, params, callback) {
        //console.log("init mobile");
      },

      load: function (params, callback) {
        //console.log("load");
      },

      loadMobile: function (params, callback) {
        //console.log("load mobile");
      }
    };
  }
);


