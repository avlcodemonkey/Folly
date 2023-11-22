module.exports = {
    /* See all the pre-defined configs here: https://www.npmjs.com/package/eslint-config-defaults */
    "extends": [
        "eslint-config-airbnb-base"
    ],
    "plugins": [
        "import"
    ],
    "env": {
        "browser": true,
        "amd": true,
        "es6": true
    },
    "parserOptions": {
        "sourceType": "module",
        "ecmaVersion": "latest"
    },
    "rules": {
        "indent": [
            "error",
            4
        ],
        "lines-between-class-members": "off",
        "space-before-function-paren": "off",
        "linebreak-style": [
            "error",
            "windows"
        ],
        "curly": [
            "error",
            "all"
        ],
        "max-len": ["error", { "code": 160 }],
        "no-underscore-dangle": [ "error", { "allow": [ "_index" ] } ]
    },
    "ignorePatterns": [ ".eslintrc.js", "node_modules", "wwwroot" ]
}
