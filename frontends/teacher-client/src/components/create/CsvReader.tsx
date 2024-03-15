import {ChangeEvent, forwardRef} from "react";
import {parse, ParseConfig} from "papaparse";
import {Button, styled} from "@mui/material";
import {UploadRounded} from "@mui/icons-material";
import PropTypes from "prop-types";

export interface IFileInfo {
    name: string
    size: number
    type: string
}

export interface CSVReaderProps {
    accept?: string
    fileEncoding?: string
    inputId?: string
    inputName?: string
    onError?: (error: Error) => void
    onFileLoaded: (data: Array<any>, fileInfo: IFileInfo, originalFile?: File) => any
    parserOptions?: ParseConfig
    disabled?: boolean
    strict?: boolean
}

const VisuallyHiddenInput = styled('input')({
    clip: 'rect(0 0 0 0)',
    clipPath: 'inset(50%)',
    height: 1,
    overflow: 'hidden',
    position: 'absolute',
    bottom: 0,
    left: 0,
    whiteSpace: 'nowrap',
    width: 1,
});


const CSVReader = forwardRef<HTMLInputElement, CSVReaderProps>(
    (
        {
            accept = '.csv, text/csv',
            fileEncoding = 'UTF-8',
            inputId = 'react-csv-reader-input',
            inputName = 'react-csv-reader-input',
            onError = () => {
            },
            onFileLoaded,
            parserOptions = {} as ParseConfig,
            disabled = false,
            strict = false,
        },
        inputRef,
    ) => {
        const handleChangeFile = (e: ChangeEvent<HTMLInputElement>) => {
            let reader: FileReader = new FileReader()
            const files: FileList = e.target.files!

            if (files.length > 0) {
                const fileInfo: IFileInfo = {
                    name: files[0].name,
                    size: files[0].size,
                    type: files[0].type,
                }

                if (strict && accept.indexOf(fileInfo.type) <= 0) {
                    onError(new Error(`[strict mode] Accept type not respected: got '${fileInfo.type}' but not in '${accept}'`))
                    return
                }

                reader.onload = (_event: Event) => {
                    const csvData = parse(
                        reader.result as string,
                        Object.assign(parserOptions, {
                            error: onError,
                            encoding: fileEncoding,
                        }),
                    )
                    onFileLoaded(csvData?.data ?? [], fileInfo, files[0])
                }

                reader.readAsText(files[0], fileEncoding)
            }
        }

        return (
            <Button component="label" color={'success'} variant={'outlined'} startIcon={<UploadRounded/>}>
                Tải tệp lên
                <VisuallyHiddenInput
                    type="file"
                    id={inputId}
                    name={inputName}
                    accept={accept}
                    onChange={handleChangeFile}
                    disabled={disabled}
                    ref={inputRef}
                />
            </Button>

        )
    },
)

CSVReader.propTypes = {
    accept: PropTypes.string,
    fileEncoding: PropTypes.string,
    inputId: PropTypes.string,
    inputName: PropTypes.string,
    onError: PropTypes.func,
    onFileLoaded: PropTypes.func.isRequired,
    parserOptions: PropTypes.object,
    disabled: PropTypes.bool,
    strict: PropTypes.bool,
}

export default CSVReader;
