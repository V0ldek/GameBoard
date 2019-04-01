/// <binding BeforeBuild="lib" AfterBuild="default" Clean="clean" />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp");
var del = require("del");

var paths = {
    scripts: ["Scripts/**/*.js", "Scripts/**/*.ts", "Scripts/**/*.map"],
    libs: [
        "node_modules/bootstrap/**/*",
        "node_modules/jquery/**/*",
        "node_modules/jquery-ajax-unobtrusive/**/*",
        "node_modules/font-awesome/**/*",
        "node_modules/malihu-custom-scrollbar-plugin/**/*",
        "node_modules/jquery-validation/**/*",
        "node_modules/jquery-validation-unobtrusive/**/*"
    ]
};

gulp.task("lib", function() {
    gulp.src(paths.libs, { base: "node_modules" }).pipe(gulp.dest("wwwroot/lib/"));
});

gulp.task("clean", function () {
    return del(["wwwroot/scripts/**/*"]);
});

gulp.task("default", function () {
    gulp.src(paths.scripts).pipe(gulp.dest("wwwroot/scripts/"));
});