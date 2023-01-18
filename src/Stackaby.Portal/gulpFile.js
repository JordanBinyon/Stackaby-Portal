const { src, dest, series } = require('gulp');
const autoprefixer = require('autoprefixer');
const concat = require('gulp-concat');
const del = require('del');
const eslint = require('gulp-eslint');
const plumber = require('gulp-plumber');
const postcss = require('gulp-postcss');
const sass = require('gulp-sass')(require('sass'));
const sourcemaps = require('gulp-sourcemaps');

// existing site files
const targetCssFile = './wwwroot/css/site.css';
const targetJsFile = './wwwroot/js/site.js';

// where to find sass code
const sassSource = './wwwroot/css/scss/site.scss';
const cssOutput = './wwwroot/css';
const jsSource = './wwwroot/js/*.js';
const concatenatedJsFileName = 'site.js';
const jsOutput = './wwwroot/js';

function clean() {
    return del([targetCssFile, targetJsFile]);
}

function compileCss() {
    return src(sassSource)
        .pipe(sourcemaps.init())
        .pipe(plumber())
        .pipe(sass().on('error', sass.logError))
        .pipe(postcss([autoprefixer()]))
        .pipe(sourcemaps.write('.'))
        .pipe(dest(cssOutput));
}

function compileJs() {
    return src(jsSource)
        .pipe(eslint())
        .pipe(eslint.format())
        .pipe(eslint.failAfterError())
        .pipe(concat(concatenatedJsFileName))
        .pipe(dest(jsOutput));
}

exports.default = series(clean, compileCss, compileJs);