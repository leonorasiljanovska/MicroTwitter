import { Component } from '@angular/core';
import { PostService } from 'src/app/services/post.service';
import { Post } from 'src/app/models/post';
import { AuthenticationService } from 'src/app/services/authentication.service';
@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent {
posts: Post[] = [];
newContent: string = '';
activeTab: 'all' | 'my-posts' = 'all';
newImageUrl: string = ''; 
constructor(private postService: PostService, public authService: AuthenticationService){
}

loadPosts(){
  this.postService.getAllPosts()
  .subscribe({
    next: (data)=> this.posts=data,
    error: (err)=>console.error('Error fetching posts', err)
  });

}
ngOnInit(): void {
  this.loadPosts();
}

addPost(){

   if (this.newContent.trim().length <12 || this.newContent.trim().length >140){
    alert("Post content must be between 12 and 140 characters.");
    return;
   }

   const newPost: Partial<Post>={
    content: this.newContent,
    imageUrl: this.newImageUrl ? this.newImageUrl : undefined
   };

   this.postService.createPost(newPost)
   .subscribe({
      next:(post:Post) => {
        console.log('Created post:', post);
          console.log('Post username:', post.username);
          console.log('Current username from service:', this.authService.getCurrentUsername());
          console.log('Are they equal?', post.username === this.authService.getCurrentUsername());
        this.posts.unshift(post);
        this.newContent='';
        this.removeImageUrl();
      },
      error: (err)=> console.error('Error creating post', err)
   });
}
removeImageUrl() {
  this.newImageUrl = '';
}
deletePost(id: string){
  this.postService.deletePost(id)
  .subscribe({
    next:() => this.posts= this.posts.filter(post => post.id !== id),
    error: (err)=> console.error('Error deleting post', err)
  });
}

get filteredPosts(): Post[] {
    if (this.activeTab === 'my-posts') {
      const currentUsername = this.authService.getCurrentUsername();
      console.log('Active tab:', this.activeTab);
      console.log('Current username:', currentUsername);
      console.log('All posts:', this.posts);
  
      return this.posts.filter(post => post.username === currentUsername);
      
    }
    return this.posts;
  }

  setActiveTab(tab: 'all' | 'my-posts'): void {
    this.activeTab = tab;
  }
}