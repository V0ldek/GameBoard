/// <binding BeforeBuild="lib" AfterBuild="default" Clean="clean" />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

const gulp = require("gulp");
const del = require("del");
const concat = require("gulp-concat");
const cleanCss = require("gulp-clean-css");
const terser = require("gulp-terser");
const merge = require("merge-stream");
const bundleconfig = require("./bundleconfig.json");
const gutil = require("gulp-util");

const paths = {
    scripts: ["Scripts/**/*.js", "Scripts/**/*.ts", "Scripts/**/*.map"],
    libs: [
        "node_modules/bootstrap/**/*",
        "node_modules/jquery/**/*",
        "node_modules/jquery-ajax-unobtrusive/**/*",
        "node_modules/font-awesome/**/*",
        "node_modules/malihu-custom-scrollbar-plugin/**/*",
        "node_modules/jquery-validation/**/*",
        "node_modules/jquery-validation-unobtrusive/**/*",
        "node_modules/popper.js/**/*"
    ]
};

gulp.task("lib:all",
    () => {
        return gulp.src(paths.libs, { base: "node_modules" }).pipe(gulp.dest("wwwroot/lib/"));
    });

gulp.task("lib:popperFix",
    () => {
        // Fix for Boostrap being dumb and not seeing the type definition in popper.js.
        return gulp.src("node_modules/popper.js/index.d.ts", { base: "node_modules" })
            .pipe(gulp.dest("node_modules/@types/"));
    });

gulp.task("lib", gulp.series(["lib:all", "lib:popperFix"]));

gulp.task("clean:bundles",
    () => {
        return Promise.all(getBundles(/.*/).map(bundle => {
            return del([bundle.outputFileName]);
        }));
    });

gulp.task("clean:typescript",
    () => {
        return del(["wwwroot/scripts/**/*", "Scripts/**/*.js", "Scripts/**/*.js.map"]);
    });

gulp.task("clean", gulp.series(["clean:bundles", "clean:typescript"]));

gulp.task("min:js",
    () => {
        return merge(getBundles(/\.js$/).map(bundle => {
            gutil.log(bundle);
            return gulp.src(bundle.inputFiles, { base: "." })
                .pipe(concat(bundle.outputFileName))
                .pipe(terser())
                .pipe(gulp.dest("."));
        }));
    });

gulp.task("min:css",
    () => {
        return merge(getBundles(/\.css$/).map(bundle => {
            return gulp.src(bundle.inputFiles, { base: "." })
                .pipe(concat(bundle.outputFileName))
                .pipe(cleanCss())
                .pipe(gulp.dest("."));
        }));
    });

gulp.task("min", gulp.series(["min:js", "min:css"]));
    
gulp.task("copy",
    () => {
        return gulp.src(paths.scripts).pipe(gulp.dest("wwwroot/scripts/"));
    });

gulp.task("default", gulp.series(["min", "copy"]));

function getBundles(regexPattern) {
    return bundleconfig.filter(bundle => {
        return regexPattern.test(bundle.outputFileName);
    });
};