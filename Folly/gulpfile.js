/// <binding ProjectOpened='watch' />

var
    autoprefixer = require('autoprefixer'),
    cleancss = require('gulp-clean-css'),
    del = require('del'),
    gulp = require('gulp'),
    postcss = require('gulp-postcss'),
    sass = require('gulp-sass')(require('sass')),
    sourcemaps = require('gulp-sourcemaps'),
    webpack = require('webpack-stream'),
    webpackConfig = require('./webpack.config.js');

var paths = {
    root: './Assets/',
    js: './Assets/js/',
    css: './Assets/css/',
    font: './Assets/fontello/font/',
    dist: './wwwroot/'
};

function sassFiles() {
    return gulp.src([paths.css + 'main.scss'])
        .pipe(sourcemaps.init())
        .pipe(sass({ includePaths: ['./node_modules'] }).on('error', sass.logError))
        .pipe(postcss([autoprefixer()]))
        .pipe(cleancss())
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(paths.dist + 'css/'));
}

function jsFiles() {
    return gulp.src(paths.js + 'main.js')
        .pipe(
            webpack(webpackConfig).on('error', function(err) {
                console.error('WEBPACK ERROR', err);
                this.emit('end'); // Don't stop the rest of the task
            })
        )
        .pipe(gulp.dest(paths.dist + 'js/'));
}

// Fonts
function fonts() {
    return gulp.src([
        paths.font + '*.woff2'
    ])
        .pipe(gulp.dest(paths.dist + 'font/'));
}

function clean(done) {
    del(paths.dist + '**');
    done();
}

function favicon() {
    return gulp.src([paths.root + '*.ico']).pipe(gulp.dest(paths.dist));
}

function watchFiles() {
    gulp.watch([paths.css + '**/*.scss', paths.css + '**/*.css'], sassFiles);
    gulp.watch(paths.js + '**/*.js', jsFiles);
    gulp.watch(paths.font, fonts);
}

gulp.task('sass', sassFiles);
gulp.task('js', jsFiles);
gulp.task('fonts', fonts);
gulp.task('clean', clean);
gulp.task('favicon', favicon);
gulp.task('build', gulp.series(clean, jsFiles, sassFiles, favicon, fonts));
gulp.task('watch', watchFiles);
