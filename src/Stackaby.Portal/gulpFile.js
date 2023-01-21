const { src, dest, series } = require('gulp');
const autoprefixer = require('autoprefixer');
const del = require('del');
const plumber = require('gulp-plumber');
const postcss = require('gulp-postcss');
const sass = require('gulp-sass')(require('sass'));
const sourcemaps = require('gulp-sourcemaps');
const watch = require('gulp-watch');

// existing site files
const targetCssFile = './wwwroot/css/site.css';

// where to find sass code
const sassSource = './wwwroot/css/scss/site.scss';
const sassWatchSource = './wwwroot/css/scss/**/*.scss';
const cssOutput = './wwwroot/css';

function clean() {
    return del([targetCssFile]);
}

async function compileCss() {
    await new Promise((resolve) => {
        src(sassSource)
            .pipe(sourcemaps.init())
            .pipe(plumber())
            .pipe(sass().on('error', sass.logError))
            .pipe(postcss([autoprefixer()]))
            .pipe(sourcemaps.write('.'))
            .pipe(dest(cssOutput))
            .on('end', resolve);
    });
    
    console.log("Completed compiling css!");
}

function watchFiles() {
    return watch([sassWatchSource], async function () {
        console.log("Change detected, cleaning files");
        clean();
        
        console.log("Compiling new files...")
        await new Promise(() => {
            compileCss()
        });
    });
}

exports.default = series(clean, compileCss);
exports.watch = watchFiles;