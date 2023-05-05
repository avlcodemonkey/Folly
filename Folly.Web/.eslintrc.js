module.exports = {
    /* See all the pre-defined configs here: https://www.npmjs.com/package/eslint-config-defaults */
    "extends": [
        "airbnb-typescript/base"
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
        "ecmaVersion": "latest",
        "tsconfigRootDir": __dirname,
        "project": "./tsconfig.json"
    },
    "rules": {
        "indent": "off",
        "@typescript-eslint/indent": [
            "error",
            4
        ],
        "lines-between-class-members": "off",
        "@typescript-eslint/lines-between-class-members": "off",
        "space-before-function-paren": "off",
        "@typescript-eslint/space-before-function-paren": "off",
        "linebreak-style": [
            "error",
            "windows"
        ],
        "curly": [
            "error",
            "all"
        ]
    },
    "ignorePatterns": [ "*.js" ]
}
