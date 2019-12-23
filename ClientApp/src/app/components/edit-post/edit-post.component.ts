import { Component, OnInit, ɵɵcontainerRefreshEnd } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatTableDataSource, MatSnackBar } from '@angular/material';
import { Inject } from '@angular/core';
import { PostService } from '../../services/post.service';
import {FormBuilder, FormGroup, Validators, FormControl,AsyncValidatorFn, ValidationErrors} from '@angular/forms';
import * as _ from 'lodash';
import { Post } from 'src/app/models/post';
<<<<<<< HEAD
import { AuthenticationService } from '../../services/authentication.service';
=======
>>>>>>> 157a877127ad7a6b6911707f4b01db1f0743e6f5
import { Router } from '@angular/router';
import { MatTableState } from 'src/app/helpers/mattable.state';


@Component({
    selector: 'app-edit-post-mat',
    templateUrl: './edit-post.component.html',
    styleUrls: ['./edit-post.component.css']
})

export class EditPostComponent {
    public frm: FormGroup;
    public ctlId: FormControl;
    public ctlTitle: FormControl;
    public ctlBody: FormControl;
    public isNew: boolean;
    dataSource: MatTableDataSource<Post> = new MatTableDataSource();
    state: MatTableState;

    constructor(
        public dialogRef: MatDialogRef<EditPostComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { post: Post; isNew: boolean; },
        private fb: FormBuilder,
        public postService: PostService,
        public router: Router,
    ) {
        this.ctlTitle = this.fb.control('', 
            [
                Validators.required, 
            ]
        );

        this.ctlBody = this.fb.control('', 
            [
                Validators.required, 
            ]
        );

        this.frm = this.fb.group({
            id: this.ctlId,
            title: this.ctlTitle,
            body: this.ctlBody,
<<<<<<< HEAD
        });
=======
        }, {});
>>>>>>> 157a877127ad7a6b6911707f4b01db1f0743e6f5
        console.log(data);
        this.isNew = data.isNew;
        this.frm.patchValue(data.post);
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    update() {
        this.dialogRef.close(this.frm.value);
    }

    cancel() {
        this.dialogRef.close();
    }
}