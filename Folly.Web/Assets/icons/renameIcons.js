// Bulk rename svg files from Mono Icon, ke-bab case to Pascal case.
// `optimized/arrow-left.svg` => `optimized/_ArrowLeft.cshtml`

const fs = require('fs');
const path = require('path');

const folderPath = './optimized';

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function getPascalFromSnake(input) {
    return input.split('-').map(capitalizeFirstLetter).join('');
}

// read all files in the directory
const filesArr = fs.readdirSync(folderPath);

// Loop through array and rename all files
filesArr.forEach((file) => {
    const fullPath = path.join(folderPath, file);
    const fileName = path.basename(`_${getPascalFromSnake(file)}`, 'svg');

    const newFileName = `${fileName}cshtml`;
    try {
        console.log(newFileName);
        fs.renameSync(fullPath, path.join(folderPath, newFileName));
    } catch (error) {
        console.error(error);
    }
});
