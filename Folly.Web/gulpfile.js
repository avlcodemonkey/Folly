/// <binding ProjectOpened='watch' />

const autoprefixer = require('autoprefixer');
const cleancss = require('gulp-clean-css');
const del = require('del');
const gulp = require('gulp');
const postcss = require('gulp-postcss');
const sass = require('gulp-sass')(require('sass'));
const sourcemaps = require('gulp-sourcemaps');
const webpack = require('webpack-stream');

const webpackConfig = {
    mode: 'production',
    target: ['web'],
    output: {
        filename: 'main.js',
    },
    devtool: 'source-map',
};

const paths = {
    root: './Assets/',
    js: './Assets/js/',
    css: './Assets/css/',
    font: './Assets/mono-icons/',
    dist: './wwwroot/',
};

function sassFiles() {
    return gulp.src([`${paths.css}main.scss`])
        .pipe(sourcemaps.init())
        .pipe(sass({ includePaths: ['./node_modules'] }).on('error', sass.logError))
        .pipe(postcss([autoprefixer()]))
        .pipe(cleancss())
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(`${paths.dist}css/`));
}

function jsFiles() {
    // @ts-ignore
    return gulp.src(`${paths.js}main.js`).pipe(webpack(webpackConfig).on('error', (err) => {
        // eslint-disable-next-line no-console
        console.error('WEBPACK ERROR', err);
        this.emit('end');
    })).pipe(gulp.dest(`${paths.dist}js/`));
}

// Fonts
function fonts() {
    return gulp.src([`${paths.font}*.woff2`]).pipe(gulp.dest(`${paths.dist}font/`));
}

function clean(done) {
    del(`${paths.dist}**`);
    done();
}

function favicon() {
    return gulp.src([`${paths.root}*.ico`]).pipe(gulp.dest(paths.dist));
}

function watchFiles() {
    gulp.watch([`${paths.css}**/*.scss`, `${paths.css}**/*.css`], sassFiles);
    gulp.watch(`${paths.js}**/*.js`, jsFiles);
    gulp.watch(paths.font, fonts);
}

gulp.task('sass', sassFiles);
gulp.task('js', jsFiles);
gulp.task('fonts', fonts);
gulp.task('clean', clean);
gulp.task('favicon', favicon);
gulp.task('build', gulp.series(clean, jsFiles, sassFiles, favicon, fonts));
gulp.task('watch', watchFiles);
